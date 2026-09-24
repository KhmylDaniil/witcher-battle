import { Navigate, Route, BrowserRouter, Routes } from 'react-router-dom'
import { LoginPage } from './features/auth/LoginPage'
import { RegisterPage } from './features/auth/RegisterPage'
import { BattleMapEditorPage } from './features/battleMaps/BattleMapEditorPage'
import { BattleDetailsPage } from './features/battles/BattleDetailsPage'
import { BodyTemplateDetailsPage } from './features/bodyTemplates/BodyTemplateDetailsPage'
import { BodyTemplatesListPage } from './features/bodyTemplates/BodyTemplatesListPage'
import { AbilityDetailsPage as CharacterAbilityDetailsPage } from './features/characters/AbilityDetailsPage'
import { AbilityFormPage as CharacterAbilityFormPage } from './features/characters/AbilityFormPage'
import { CharacterDetailsPage } from './features/characters/CharacterDetailsPage'
import { CharacterFormPage } from './features/characters/CharacterFormPage'
import { CharactersListPage } from './features/characters/CharactersListPage'
import { AbilityDetailsPage } from './features/creatureTemplates/AbilityDetailsPage'
import { AbilityFormPage } from './features/creatureTemplates/AbilityFormPage'
import { CreatureTemplateDetailsPage } from './features/creatureTemplates/CreatureTemplateDetailsPage'
import { CreatureTemplateFormPage } from './features/creatureTemplates/CreatureTemplateFormPage'
import { CreatureTemplatesListPage } from './features/creatureTemplates/CreatureTemplatesListPage'
import { GameDetailsPage } from './features/games/GameDetailsPage'
import { ItemTemplateDetailsPage } from './features/itemTemplates/ItemTemplateDetailsPage'
import { ItemTemplateFormPage } from './features/itemTemplates/ItemTemplateFormPage'
import { ItemTemplatesListPage } from './features/itemTemplates/ItemTemplatesListPage'
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
          {/* Редактор карты открывается в отдельном окне — без шапки AppLayout, на всю площадь окна. */}
          <Route path="/games/:gameId/battle-maps/:battleMapId" element={<BattleMapEditorPage />} />
          <Route element={<AppLayout />}>
            <Route path="/" element={<Navigate to="/games" replace />} />
            <Route path="/games" element={<GamesListPage />} />
            <Route path="/characters" element={<CharactersListPage />} />
            <Route path="/characters/:characterId" element={<CharacterDetailsPage />} />
            <Route path="/characters/:characterId/abilities/new" element={<CharacterAbilityFormPage />} />
            <Route path="/characters/:characterId/abilities/:abilityId" element={<CharacterAbilityDetailsPage />} />
            <Route path="/games/:gameId" element={<GameDetailsPage />} />
            <Route path="/games/:gameId/characters/new" element={<CharacterFormPage />} />
            <Route path="/games/:gameId/characters/:characterId" element={<CharacterDetailsPage />} />
            <Route path="/games/:gameId/characters/:characterId/edit" element={<CharacterFormPage />} />
            <Route path="/games/:gameId/creature-templates" element={<CreatureTemplatesListPage />} />
            <Route path="/games/:gameId/creature-templates/new" element={<CreatureTemplateFormPage />} />
            <Route path="/games/:gameId/creature-templates/:creatureTemplateId" element={<CreatureTemplateDetailsPage />} />
            <Route path="/games/:gameId/creature-templates/:creatureTemplateId/abilities/new" element={<AbilityFormPage />} />
            <Route path="/games/:gameId/creature-templates/:creatureTemplateId/abilities/:abilityId" element={<AbilityDetailsPage />} />
            <Route path="/games/:gameId/body-templates" element={<BodyTemplatesListPage />} />
            <Route path="/games/:gameId/body-templates/:bodyTemplateId" element={<BodyTemplateDetailsPage />} />
            <Route path="/games/:gameId/item-templates" element={<ItemTemplatesListPage />} />
            <Route path="/games/:gameId/item-templates/new" element={<ItemTemplateFormPage />} />
            <Route path="/games/:gameId/item-templates/:itemTemplateId" element={<ItemTemplateDetailsPage />} />
            <Route path="/games/:gameId/battles/:battleId" element={<BattleDetailsPage />} />
          </Route>
        </Route>

        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  )
}
