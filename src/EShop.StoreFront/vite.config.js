import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import p from "path";
import tailwindcss from "@tailwindcss/vite";

export default defineConfig(({ mode }) => {
  return {
    plugins: [react(), tailwindcss()],
    server: {
      port: 3000,
      host: true,
    },
    build: {
      outDir: "dist",
      rollupOptions: {
        input: "./index.html",
      },
    },

    resolve: {
      alias: {
        "@": p.resolve(__dirname, "./src"),
        "@pages": p.resolve(__dirname, "./src/pages"),
        "@app": p.resolve(__dirname, "./src/app"),
        "@features": p.resolve(__dirname, "./src/features"),
        "@components": p.resolve(__dirname, "./src/components"),
        "@lib": p.resolve(__dirname, "./src/lib"),
        "@hooks": p.resolve(__dirname, "./src/hooks"),
        "@env": p.resolve(__dirname, "./src/env"),
        "@public": p.resolve(__dirname, "./src/public"),
      },
    },
    css: {
      modules: {
        generateScopedName: "[hash:base64:6]",
      },
    },
  };
});
