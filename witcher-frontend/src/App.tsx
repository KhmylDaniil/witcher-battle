import { Navigate, Route, BrowserRouter, Routes } from 'react-router-dom'
import { LoginPage } from './features/auth/LoginPage'
import { RegisterPage } from './features/auth/RegisterPage'
import { CharacterDetailsPage } from './features/characters/CharacterDetailsPage'
import { CharacterFormPage } from './features/characters/CharacterFormPage'
import { CharactersListPage } from './features/characters/CharactersListPage'
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
            <Route path="/" element={<Navigate to="/characters" replace />} />
            <Route path="/characters" element={<CharactersListPage />} />
            <Route path="/characters/new" element={<CharacterFormPage />} />
            <Route path="/characters/:characterId" element={<CharacterDetailsPage />} />
            <Route path="/characters/:characterId/edit" element={<CharacterFormPage />} />
          </Route>
        </Route>

        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  )
}
