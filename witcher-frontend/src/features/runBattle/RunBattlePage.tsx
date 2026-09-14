import { useCallback, useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Link, useParams } from 'react-router-dom'
import { Button, Card, ErrorText, PageHeader, Spinner } from '../../components/ui'
import { ApiError } from '../../lib/apiClient'
import { useBattleUpdates } from '../../lib/signalr'
import { useGameId } from '../../routes/GameLayout'
import { AttackForm } from './AttackForm'
import { HealForm } from './HealForm'
import { runBattleApi } from './api'

type ActionMode = 'idle' | 'attack' | 'heal'

export function RunBattlePage() {
  const gameId = useGameId()
  const { battleId } = useParams<{ battleId: string }>()
  const queryClient = useQueryClient()
  const [action, setAction] = useState<ActionMode>('idle')

  const runKey = ['run-battle', gameId, battleId] as const
  const run = useQuery({ queryKey: runKey, queryFn: () => runBattleApi.run(gameId, battleId!) })

  const invalidateRun = useCallback(() => queryClient.invalidateQueries({ queryKey: runKey }), [queryClient, runKey])
  useBattleUpdates(invalidateRun)

  const creatureId = run.data?.creatureId
  const turn = useQuery({
    queryKey: ['run-battle-turn', gameId, battleId, creatureId],
    queryFn: () => runBattleApi.makeTurn(gameId, battleId!, creatureId!),
    enabled: !!creatureId,
  })

  const passInMultiAttack = useMutation({
    mutationFn: () => runBattleApi.passInMultiAttack(gameId, battleId!, creatureId!),
    onSuccess: invalidateRun,
  })
  const endTurn = useMutation({
    mutationFn: () => runBattleApi.endTurn(gameId, battleId!, creatureId!),
    onSuccess: () => {
      invalidateRun()
      setAction('idle')
    },
  })

  if (run.isLoading) return <Spinner />
  if (!run.data) return null
  const battle = run.data

  const canAct = turn.data && turn.data.turnState !== 'TurnNotBeginned' && turn.data.turnState !== 'TurnIsDone'
  const canAttack = canAct && (Object.keys(turn.data!.myAbilities).length > 0 || Object.keys(turn.data!.equippedWeapons).length > 0)
  const canHeal = turn.data?.turnState === 'ReadyForAction'
  const canPassMultiattack = turn.data?.turnState === 'InProcessOfBaseAction' || turn.data?.turnState === 'InProcessOfAdditionAction'

  return (
    <div className="flex flex-col gap-4">
      <PageHeader
        title={battle.name}
        actions={
          <Link to={`/games/${gameId}/battles/${battleId}`}>
            <Button variant="secondary">К бою</Button>
          </Link>
        }
      />

      <Card>
        <h2 className="mb-2 font-semibold">Ход: {battle.currentCreatureName}</h2>
        <div className="overflow-x-auto">
          <table className="w-full text-left text-sm">
            <thead>
              <tr className="border-b border-neutral-200 text-neutral-500 dark:border-neutral-800">
                <th className="py-2 pr-3">Имя</th>
                <th className="py-2 pr-3">HP</th>
                <th className="py-2 pr-3">Состояния</th>
                <th className="py-2 pr-3">Инициатива</th>
              </tr>
            </thead>
            <tbody>
              {battle.creatures.map((c) => (
                <tr
                  key={c.id}
                  className={`border-b border-neutral-100 dark:border-neutral-900 ${
                    c.id === battle.creatureId ? 'bg-violet-50 dark:bg-violet-950/40' : ''
                  }`}
                >
                  <td className="py-2 pr-3 font-medium">{c.name}</td>
                  <td className="py-2 pr-3">
                    {c.hp.current}/{c.hp.max}
                  </td>
                  <td className="py-2 pr-3">{c.effects || '—'}</td>
                  <td className="py-2 pr-3">{c.initiative}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </Card>

      <Card>
        <h2 className="mb-2 font-semibold">Действие</h2>
        {turn.isLoading && <Spinner />}
        {turn.data && action === 'idle' && (
          <div className="flex flex-wrap gap-2">
            {canAttack && <Button onClick={() => setAction('attack')}>Атаковать</Button>}
            {canHeal && (
              <Button variant="secondary" onClick={() => setAction('heal')}>
                Лечить / снять эффект
              </Button>
            )}
            {canPassMultiattack && (
              <Button variant="secondary" disabled={passInMultiAttack.isPending} onClick={() => passInMultiAttack.mutate()}>
                Пропустить мультиатаку
              </Button>
            )}
            <Button variant="secondary" disabled={endTurn.isPending} onClick={() => endTurn.mutate()}>
              Завершить ход
            </Button>
          </div>
        )}
        {endTurn.error && <ErrorText>{endTurn.error instanceof ApiError ? endTurn.error.message : 'Не удалось завершить ход'}</ErrorText>}
        {passInMultiAttack.error && (
          <ErrorText>{passInMultiAttack.error instanceof ApiError ? passInMultiAttack.error.message : 'Не удалось пропустить'}</ErrorText>
        )}

        {turn.data && action === 'attack' && (
          <AttackForm gameId={gameId} battleId={battleId!} turn={turn.data} onDone={() => setAction('idle')} onCancel={() => setAction('idle')} />
        )}
        {turn.data && action === 'heal' && (
          <HealForm gameId={gameId} battleId={battleId!} turn={turn.data} onDone={() => setAction('idle')} onCancel={() => setAction('idle')} />
        )}
      </Card>

      <Card>
        <h2 className="mb-2 font-semibold">Журнал боя</h2>
        <pre className="max-h-96 overflow-y-auto whitespace-pre-wrap text-xs text-neutral-600 dark:text-neutral-400">{battle.battleLog}</pre>
      </Card>
    </div>
  )
}
