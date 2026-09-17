using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using Wastelands.Service.Application.Contracts;
using Wastelands.Service.Application.Options;

namespace Wastelands.Service.Infrastructure.Storage
{
	public class MinioImageStorage : IImageStorage
	{
		private readonly IMinioClient _client;
		private readonly string _bucket;

		public MinioImageStorage(IMinioClient client, IOptions<MinioOptions> options)
		{
			_client = client;
			_bucket = options.Value.Bucket;
		}

		public async Task<string> UploadAsync(Stream content, string contentType, CancellationToken cancellationToken = default)
		{
			var key = Guid.NewGuid().ToString("N");

			await _client.PutObjectAsync(
				new PutObjectArgs()
					.WithBucket(_bucket)
					.WithObject(key)
					.WithStreamData(content)
					.WithObjectSize(content.Length)
					.WithContentType(contentType),
				cancellationToken).ConfigureAwait(false);

			return key;
		}

		public async Task<(Stream Content, string ContentType)?> DownloadAsync(string key, CancellationToken cancellationToken = default)
		{
			var buffer = new MemoryStream();

			try
			{
				var stat = await _client.GetObjectAsync(
					new GetObjectArgs()
						.WithBucket(_bucket)
						.WithObject(key)
						.WithCallbackStream((stream, ct) => stream.CopyToAsync(buffer, ct)),
					cancellationToken).ConfigureAwait(false);

				buffer.Position = 0;
				return (buffer, stat.ContentType);
			}
			catch (ObjectNotFoundException)
			{
				return null;
			}
		}

		public async Task DeleteAsync(string key, CancellationToken cancellationToken = default)
		{
			await _client.RemoveObjectAsync(
				new RemoveObjectArgs().WithBucket(_bucket).WithObject(key),
				cancellationToken).ConfigureAwait(false);
		}

		public async Task EnsureBucketExistsAsync(CancellationToken cancellationToken = default)
		{
			var exists = await _client.BucketExistsAsync(
				new BucketExistsArgs().WithBucket(_bucket),
				cancellationToken).ConfigureAwait(false);

			if (!exists)
			{
				await _client.MakeBucketAsync(
					new MakeBucketArgs().WithBucket(_bucket),
					cancellationToken).ConfigureAwait(false);
			}
		}
	}
}
