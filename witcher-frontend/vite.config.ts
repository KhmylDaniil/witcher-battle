import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'
import { defineConfig } from 'vite'

// Backend dev URL from Wastelands.Service.MVC/Properties/launchSettings.json. Must be the HTTPS port —
// Program.cs runs app.UseHttpsRedirection() unconditionally, so proxying to the HTTP port just bounces
// every request straight back to this HTTPS one via a 307, and the browser then follows that redirect
// as a genuine cross-origin request with no CORS headers to satisfy it (see git history for the CORS
// bug this caused when this was still pointed at Witcher.MVC on the http port).
// secure: false trusts the backend's self-signed ASP.NET Core dev certificate (dotnet dev-certs https).
const backendDevUrl = 'https://localhost:7114'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss()],
  server: {
    proxy: {
      '/api': { target: backendDevUrl, changeOrigin: true, secure: false },
    },
  },
})
