import { useForm } from 'react-hook-form'
import { Link, useNavigate } from 'react-router-dom'
import { Button, Card, ErrorText, Field, Input } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { useLogin } from './useAuth'
import type { LoginPayload } from './api'

export function LoginPage() {
  const navigate = useNavigate()
  const login = useLogin()
  const { register, handleSubmit, formState } = useForm<LoginPayload>()

  const onSubmit = handleSubmit(async (values) => {
    try {
      await login.mutateAsync(values)
      navigate('/games', { replace: true })
    } catch {
      // ошибка уже доступна через login.error ниже
    }
  })

  return (
    <div className="mx-auto flex min-h-screen max-w-sm flex-col justify-center gap-4 px-4">
      <h1 className="text-center text-2xl font-semibold">Witcher Battle</h1>
      <Card>
        <form onSubmit={onSubmit} className="flex flex-col gap-3">
          <Field label="Логин">
            <Input {...register('login', { required: true })} autoFocus />
          </Field>
          <Field label="Пароль">
            <Input type="password" {...register('password', { required: true })} />
          </Field>
          {login.error && (
            <ErrorText>{login.error instanceof ApiError ? login.error.message : 'Не удалось войти'}</ErrorText>
          )}
          <Button type="submit" disabled={formState.isSubmitting || login.isPending}>
            Войти
          </Button>
        </form>
      </Card>
      <p className="text-center text-sm text-neutral-500">
        Нет аккаунта? <Link to="/register" className="text-violet-600 hover:underline">Зарегистрироваться</Link>
      </p>
    </div>
  )
}
