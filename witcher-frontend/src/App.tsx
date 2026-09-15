import { Navigate, Route, BrowserRouter, Routes } from 'react-router-dom'
import { LoginPage } from './features/auth/LoginPage'
import { RegisterPage } from './features/auth/RegisterPage'
import { BodyTemplateDetailsPage } from './features/bodyTemplates/BodyTemplateDetailsPage'
import { CharacterDetailsPage } from './features/characters/CharacterDetailsPage'
import { CharacterFormPage } from './features/characters/CharacterFormPage'
import { CreatureTemplateDetailsPage } from './features/creatureTemplates/CreatureTemplateDetailsPage'
import { CreatureTemplateFormPage } from './features/creatureTemplates/CreatureTemplateFormPage'
import { GameDetailsPage } from './features/games/GameDetailsPage'
import { GamesListPage } from './features/games/GamesListPage'
import { AppLayout } from './routes/AppLayout'
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
            <Route path="/games/:gameId" element={<GameDetailsPage />} />
            <Route path="/games/:gameId/characters/new" element={<CharacterFormPage />} />
            <Route path="/games/:gameId/characters/:characterId" element={<CharacterDetailsPage />} />
            <Route path="/games/:gameId/characters/:characterId/edit" element={<CharacterFormPage />} />
            <Route path="/games/:gameId/creature-templates/new" element={<CreatureTemplateFormPage />} />
            <Route path="/games/:gameId/creature-templates/:creatureTemplateId" element={<CreatureTemplateDetailsPage />} />
            <Route path="/games/:gameId/body-templates/:bodyTemplateId" element={<BodyTemplateDetailsPage />} />
          </Route>
        </Route>

        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  )
}
