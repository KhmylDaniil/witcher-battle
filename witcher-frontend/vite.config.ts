import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'
import { defineConfig } from 'vite'

// Backend dev URL from Witcher.MVC/Properties/launchSettings.json. Must be the HTTPS port —
// Program.cs runs app.UseHttpsRedirection() unconditionally, so proxying to the HTTP port just
// bounces every request straight back to this HTTPS one via a 307. Vite would forward that redirect
// response as-is, and the browser would then follow it as a genuine cross-origin request (5173 -> 7277)
// with no CORS headers to satisfy it — which is the "blocked by CORS policy" error this avoids.
// secure: false trusts the backend's self-signed ASP.NET Core dev certificate (dotnet dev-certs https).
const backendDevUrl = 'https://localhost:7277'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss()],
  server: {
    proxy: {
      '/api': { target: backendDevUrl, changeOrigin: true, secure: false },
      '/messageHub': { target: backendDevUrl, changeOrigin: true, secure: false, ws: true },
    },
  },
})
