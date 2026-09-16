import type { ButtonHTMLAttributes, InputHTMLAttributes, ReactNode, SelectHTMLAttributes, TextareaHTMLAttributes } from 'react'

export function Button({
  variant = 'primary',
  className = '',
  ...props
}: ButtonHTMLAttributes<HTMLButtonElement> & { variant?: 'primary' | 'secondary' | 'danger' }) {
  const base = 'inline-flex items-center justify-center rounded-md px-3 py-1.5 text-sm font-medium transition disabled:opacity-50 disabled:pointer-events-none'
  const styles = {
    primary: 'bg-violet-600 text-white hover:bg-violet-700',
    secondary: 'bg-neutral-200 text-neutral-900 hover:bg-neutral-300 dark:bg-neutral-800 dark:text-neutral-100 dark:hover:bg-neutral-700',
    danger: 'bg-red-600 text-white hover:bg-red-700',
  }
  return <button className={`${base} ${styles[variant]} ${className}`} {...props} />
}

/**
 * Кнопка удаления с браузерным confirm() перед вызовом действия — единая реализация вместо
 * повторяющегося `if (confirm(msg)) mutate()` в каждой карточке. `link` переключает вид на
 * текстовую ссылку (для рядов таблиц), иначе рендерится как обычный <Button variant="danger">.
 */
export function ConfirmButton({
  confirmMessage,
  onConfirm,
  link = false,
  className = '',
  children,
  ...props
}: ButtonHTMLAttributes<HTMLButtonElement> & {
  confirmMessage: string
  onConfirm: () => void
  link?: boolean
}) {
  const handleClick = () => {
    if (confirm(confirmMessage)) onConfirm()
  }

  if (link) {
    return (
      <button className={`text-red-600 hover:underline ${className}`} onClick={handleClick} {...props}>
        {children}
      </button>
    )
  }

  return (
    <Button variant="danger" className={className} onClick={handleClick} {...props}>
      {children}
    </Button>
  )
}

export function Field({ label, children }: { label: string; children: ReactNode }) {
  return (
    <label className="flex flex-col gap-1 text-sm">
      <span className="font-medium text-neutral-700 dark:text-neutral-300">{label}</span>
      {children}
    </label>
  )
}

const inputBase =
  'rounded-md border border-neutral-300 bg-white px-2.5 py-1.5 text-sm text-neutral-900 outline-none focus:border-violet-500 focus:ring-1 focus:ring-violet-500 dark:border-neutral-700 dark:bg-neutral-900 dark:text-neutral-100'

export function Input(props: InputHTMLAttributes<HTMLInputElement>) {
  return <input {...props} className={`${inputBase} ${props.className ?? ''}`} />
}

export function Textarea(props: TextareaHTMLAttributes<HTMLTextAreaElement>) {
  return <textarea {...props} className={`${inputBase} ${props.className ?? ''}`} />
}

export function Select(props: SelectHTMLAttributes<HTMLSelectElement>) {
  return <select {...props} className={`${inputBase} ${props.className ?? ''}`} />
}

export function Card({ children, className = '' }: { children: ReactNode; className?: string }) {
  return (
    <div className={`rounded-lg border border-neutral-200 bg-white p-4 shadow-sm dark:border-neutral-800 dark:bg-neutral-900 ${className}`}>
      {children}
    </div>
  )
}

export function PageHeader({ title, actions }: { title: string; actions?: ReactNode }) {
  return (
    <div className="mb-4 flex flex-wrap items-center justify-between gap-2">
      <h1 className="text-xl font-semibold">{title}</h1>
      {actions && <div className="flex gap-2">{actions}</div>}
    </div>
  )
}

export function ErrorText({ children }: { children: ReactNode }) {
  return <p className="rounded-md bg-red-50 px-3 py-2 text-sm text-red-700 dark:bg-red-950 dark:text-red-300">{children}</p>
}

export function Spinner() {
  return <div className="py-8 text-center text-sm text-neutral-500">Загрузка…</div>
}
