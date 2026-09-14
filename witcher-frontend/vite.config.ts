import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'
import { defineConfig } from 'vite'

// Backend dev URL from Witcher.MVC/Properties/launchSettings.json (http profile — avoids the
// self-signed dev cert on the https port). Proxying makes the SPA look same-origin to the browser
// in dev, so the existing cookie auth works with zero CORS configuration — see plan doc for why.
const backendDevUrl = 'http://localhost:5277'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss()],
  server: {
    proxy: {
      '/api': { target: backendDevUrl, changeOrigin: true },
      '/messageHub': { target: backendDevUrl, changeOrigin: true, ws: true },
    },
  },
})
