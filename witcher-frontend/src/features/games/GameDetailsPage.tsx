import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Card, PageHeader, Spinner } from '../../components/ui'
import { useGameId } from '../../routes/GameLayout'
import { useCurrentUser } from '../auth/useAuth'
import { gamesApi } from './api'

export function GameDetailsPage() {
  const gameId = useGameId()
  const queryClient = useQueryClient()
  const { data: user } = useCurrentUser()

  const game = useQuery({ queryKey: ['games', gameId], queryFn: () => gamesApi.get(gameId) })

  const toggleRole = useMutation({
    mutationFn: (userId: string) => gamesApi.changeMemberRole(gameId, userId),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['games', gameId] }),
  })
  const removeMember = useMutation({
    mutationFn: ({ userId, name }: { userId: string; name: string }) => gamesApi.removeMember(gameId, userId, name),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['games', gameId] }),
  })

  if (game.isLoading) return <Spinner />
  if (!game.data) return null

  const isMainMaster = game.data.gameMasterName && user && game.data.members.some((m) => m.userId === user.userId && m.roleName === 'MainMaster')

  return (
    <div className="flex flex-col gap-4">
      <PageHeader title={game.data.name} />
      <Card>
        <p className="text-sm text-neutral-500">{game.data.description || 'Без описания'}</p>
        <p className="mt-2 text-xs text-neutral-400">Мастер: {game.data.gameMasterName}</p>
      </Card>

      <Card>
        <h2 className="mb-3 font-semibold">Участники</h2>
        <ul className="flex flex-col gap-2">
          {game.data.members.map((member) => (
            <li key={member.userId} className="flex items-center justify-between text-sm">
              <span>
                {member.name} <span className="text-neutral-400">— {member.roleName}</span>
              </span>
              {isMainMaster && member.roleName !== 'MainMaster' && (
                <span className="flex gap-2">
                  <button
                    className="text-violet-600 hover:underline"
                    onClick={() => toggleRole.mutate(member.userId)}
                    disabled={toggleRole.isPending}
                  >
                    Сменить роль
                  </button>
                  <button
                    className="text-red-600 hover:underline"
                    onClick={() => removeMember.mutate({ userId: member.userId, name: member.name })}
                    disabled={removeMember.isPending}
                  >
                    Удалить
                  </button>
                </span>
              )}
            </li>
          ))}
        </ul>
      </Card>
    </div>
  )
}
