/** Значения из `all`, которых ещё нет в `used` — общий "used vs available" паттерн для пикеров навыков/типов урона. */
export function getAvailableOptions<T>(all: readonly T[], used: ReadonlySet<T>): T[] {
  return all.filter((x) => !used.has(x))
}
