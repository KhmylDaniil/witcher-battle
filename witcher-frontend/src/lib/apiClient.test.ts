import { afterEach, describe, expect, it, vi } from 'vitest'
import { ApiError, api, buildQuery } from './apiClient'

function jsonResponse(status: number, body: unknown) {
  return new Response(body === undefined ? '' : JSON.stringify(body), { status })
}

describe('buildQuery', () => {
  it('skips undefined, null and empty-string values', () => {
    expect(buildQuery({ a: undefined, b: null, c: '', d: 0 })).toBe('?d=0')
  })

  it('returns an empty string when nothing is left', () => {
    expect(buildQuery({ a: undefined, b: null, c: '' })).toBe('')
  })

  it('serializes multiple params in insertion order', () => {
    expect(buildQuery({ pageNumber: 2, pageSize: 10 })).toBe('?pageNumber=2&pageSize=10')
  })
})

describe('api', () => {
  afterEach(() => {
    vi.unstubAllGlobals()
  })

  it('get() resolves with the parsed JSON body', async () => {
    const fetchMock = vi.fn().mockResolvedValue(jsonResponse(200, { id: 1 }))
    vi.stubGlobal('fetch', fetchMock)

    await expect(api.get('/api/games/1')).resolves.toEqual({ id: 1 })
    expect(fetchMock).toHaveBeenCalledWith(
      '/api/games/1',
      expect.objectContaining({ credentials: 'same-origin', headers: expect.objectContaining({ 'Content-Type': 'application/json' }) }),
    )
  })

  it('resolves with undefined on 204 No Content', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue(new Response(null, { status: 204 })))

    await expect(api.delete('/api/games/1')).resolves.toBeUndefined()
  })

  it('post() JSON-encodes the body', async () => {
    const fetchMock = vi.fn().mockResolvedValue(jsonResponse(200, { ok: true }))
    vi.stubGlobal('fetch', fetchMock)

    await api.post('/api/games', { name: 'Test' })

    expect(fetchMock).toHaveBeenCalledWith('/api/games', expect.objectContaining({ method: 'POST', body: JSON.stringify({ name: 'Test' }) }))
  })

  it('upload() sends FormData without an explicit Content-Type header', async () => {
    const fetchMock = vi.fn().mockResolvedValue(jsonResponse(200, { key: 'abc' }))
    vi.stubGlobal('fetch', fetchMock)

    const file = new File(['data'], 'photo.png', { type: 'image/png' })
    await api.upload('/api/characters/1/image', file)

    const [, init] = fetchMock.mock.calls[0] as [string, RequestInit]
    expect(init.body).toBeInstanceOf(FormData)
    expect((init.headers as Record<string, string>)['Content-Type']).toBeUndefined()
  })

  it('throws ApiError with the server message on a non-ok response', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue(jsonResponse(400, { message: 'Имя занято' })))

    await expect(api.post('/api/games', {})).rejects.toMatchObject({ name: 'ApiError', status: 400, message: 'Имя занято' })
  })

  it('falls back to a generic message when the error body has none', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue(new Response('', { status: 500 })))

    await expect(api.get('/api/games')).rejects.toThrow('Ошибка запроса (500)')
  })
})

describe('ApiError', () => {
  it('carries the HTTP status alongside the message', () => {
    const error = new ApiError(404, 'Not found')
    expect(error).toBeInstanceOf(Error)
    expect(error.status).toBe(404)
    expect(error.message).toBe('Not found')
  })
})
