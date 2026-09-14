import { Navigate, Route, BrowserRouter, Routes } from 'react-router-dom'
import { LoginPage } from './features/auth/LoginPage'
import { RegisterPage } from './features/auth/RegisterPage'
import { GameDetailsPage } from './features/games/GameDetailsPage'
import { GamesListPage } from './features/games/GamesListPage'
import { CreatureTemplateDetailsPage } from './features/creatureTemplates/CreatureTemplateDetailsPage'
import { CreatureTemplateFormPage } from './features/creatureTemplates/CreatureTemplateFormPage'
import { CreatureTemplatesListPage } from './features/creatureTemplates/CreatureTemplatesListPage'
import { BattleDetailsPage } from './features/battles/BattleDetailsPage'
import { BattlesListPage } from './features/battles/BattlesListPage'
import { RunBattlePage } from './features/runBattle/RunBattlePage'
import { AppLayout } from './routes/AppLayout'
import { GameLayout } from './routes/GameLayout'
import { ProtectedRoute } from './routes/ProtectedRoute'

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />

        <Route element={<ProtectedRoute />}>
          <Route element={<AppLayout />}>
            <Route path="/" element={<Navigate to="/games" replace />} />
            <Route path="/games" element={<GamesListPage />} />

            <Route path="/games/:gameId" element={<GameLayout />}>
              <Route index element={<GameDetailsPage />} />

              <Route path="creature-templates" element={<CreatureTemplatesListPage />} />
              <Route path="creature-templates/new" element={<CreatureTemplateFormPage />} />
              <Route path="creature-templates/:templateId" element={<CreatureTemplateDetailsPage />} />
              <Route path="creature-templates/:templateId/edit" element={<CreatureTemplateFormPage />} />

              <Route path="battles" element={<BattlesListPage />} />
              <Route path="battles/:battleId" element={<BattleDetailsPage />} />
            </Route>

            {/* Экран боя без вкладок GameLayout — ему нужна вся ширина под таблицу/лог */}
            <Route path="/games/:gameId/battles/:battleId/run" element={<RunBattlePage />} />
          </Route>
        </Route>

        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  )
}
