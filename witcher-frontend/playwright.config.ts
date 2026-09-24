import { defineConfig, devices } from '@playwright/test'

// Bare `vite` dev server, not `preview` — vite.config.ts's `server.proxy` (which forwards /api to the
// backend) only applies to the dev server, so E2E needs the same setup a developer runs locally. The
// backend itself is not started here: CI starts it as a separate step (see .github/workflows/ci.yml),
// and locally you run `dotnet run` yourself before `npm run test:e2e`.
export default defineConfig({
  testDir: './e2e',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 1 : 0,
  workers: process.env.CI ? 1 : undefined,
  reporter: 'list',
  use: {
    baseURL: 'http://localhost:5173',
    trace: 'on-first-retry',
  },
  projects: [
    {
      name: 'chromium',
      use: {
        ...devices['Desktop Chrome'],
        // Set PW_CHROMIUM_PATH to pin an already-installed Chromium binary instead of the
        // headless-shell build `npx playwright install` fetches by default (useful in sandboxes
        // that ship a pre-installed browser at a fixed, non-standard path).
        launchOptions: process.env.PW_CHROMIUM_PATH ? { executablePath: process.env.PW_CHROMIUM_PATH } : undefined,
      },
    },
  ],
  webServer: {
    command: 'npm run dev',
    url: 'http://localhost:5173',
    reuseExistingServer: !process.env.CI,
    timeout: 30_000,
  },
})
