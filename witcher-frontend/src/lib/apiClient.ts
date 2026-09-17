// Тонкий fetch-клиент поверх Witcher.MVC/Controllers/Api/*. Бэкенд аутентифицирует через ту же cookie-схему,
// что и Razor MVC (см. план: SPA раздаётся с того же origin, поэтому credentials работают без CORS/токенов).

export class ApiError extends Error {
  status: number

  constructor(status: number, message: string) {
    super(message)
    this.name = 'ApiError'
    this.status = status
  }
}

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  // FormData (загрузка файла) не должна получать Content-Type вручную — браузер сам проставляет
  // его с корректным multipart-boundary, который иначе не воспроизвести.
  const isFormData = init?.body instanceof FormData
  const headers = isFormData ? { ...(init?.headers ?? {}) } : { 'Content-Type': 'application/json', ...(init?.headers ?? {}) }

  const res = await fetch(path, {
    credentials: 'same-origin',
    headers,
    ...init,
  })

  if (res.status === 204) return undefined as T

  const text = await res.text()
  const data = text ? JSON.parse(text) : undefined

  if (!res.ok) {
    const message = (data && typeof data === 'object' && 'message' in data ? String(data.message) : null) ?? `Ошибка запроса (${res.status})`
    throw new ApiError(res.status, message)
  }

  return data as T
}

/** Собирает query string, пропуская undefined/null/пустые строки */
export function buildQuery(params: Record<string, unknown>): string {
  const usp = new URLSearchParams()
  for (const [key, value] of Object.entries(params)) {
    if (value === undefined || value === null || value === '') continue
    usp.set(key, String(value))
  }
  const qs = usp.toString()
  return qs ? `?${qs}` : ''
}

export const api = {
  get: <T>(path: string) => request<T>(path),
  post: <T>(path: string, body?: unknown) =>
    request<T>(path, { method: 'POST', body: body === undefined ? undefined : JSON.stringify(body) }),
  put: <T>(path: string, body?: unknown) =>
    request<T>(path, { method: 'PUT', body: body === undefined ? undefined : JSON.stringify(body) }),
  delete: <T>(path: string) => request<T>(path, { method: 'DELETE' }),
  upload: <T>(path: string, file: File) => {
    const body = new FormData()
    body.append('file', file)
    return request<T>(path, { method: 'PUT', body })
  },
}
