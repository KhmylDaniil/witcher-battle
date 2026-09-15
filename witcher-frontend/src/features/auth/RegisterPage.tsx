import { useForm } from 'react-hook-form'
import { Link, useNavigate } from 'react-router-dom'
import { Button, Card, ErrorText, Field, Input } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { useRegister } from './useAuth'
import type { RegisterPayload } from './api'

export function RegisterPage() {
  const navigate = useNavigate()
  const register_ = useRegister()
  const { register, handleSubmit, formState } = useForm<RegisterPayload>()

  const onSubmit = handleSubmit(async (values) => {
    try {
      await register_.mutateAsync(values)
      navigate('/games', { replace: true })
    } catch {
      // ошибка уже доступна через register_.error ниже
    }
  })

  return (
    <div className="mx-auto flex min-h-screen max-w-sm flex-col justify-center gap-4 px-4">
      <h1 className="text-center text-2xl font-semibold">Регистрация</h1>
      <Card>
        <form onSubmit={onSubmit} className="flex flex-col gap-3">
          <Field label="Имя">
            <Input {...register('name', { required: true, maxLength: 20 })} autoFocus />
          </Field>
          <Field label="Email (необязательно)">
            <Input type="email" {...register('email', { maxLength: 50 })} />
          </Field>
          <Field label="Логин">
            <Input {...register('login', { required: true, maxLength: 25 })} />
          </Field>
          <Field label="Пароль">
            <Input type="password" {...register('password', { required: true, maxLength: 25 })} />
          </Field>
          {register_.error && (
            <ErrorText>{register_.error instanceof ApiError ? register_.error.message : 'Не удалось зарегистрироваться'}</ErrorText>
          )}
          <Button type="submit" disabled={formState.isSubmitting || register_.isPending}>
            Зарегистрироваться
          </Button>
        </form>
      </Card>
      <p className="text-center text-sm text-neutral-500">
        Уже есть аккаунт? <Link to="/login" className="text-violet-600 hover:underline">Войти</Link>
      </p>
    </div>
  )
}
