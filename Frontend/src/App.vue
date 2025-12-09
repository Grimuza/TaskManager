<template>
  <div id="app">
    <ThemeToggle />
    <router-view />
  </div>
</template>

<script setup lang="ts">
import ThemeToggle from './components/ThemeToggle.vue';
import { useTheme } from './composables/useTheme';

// Initialize theme
useTheme();
</script>

<style>
* {
  box-sizing: border-box;
}

/* Dark mode CSS variables (default) */
:root,
[data-theme="dark"] {
  --bg-primary: #0f0f0f;
  --bg-secondary: rgba(25, 25, 25, 0.6);
  --bg-card: rgba(25, 25, 25, 0.6);
  --bg-input: rgba(20, 20, 20, 0.8);
  --bg-badge: rgba(0, 217, 163, 0.12);
  --border-badge: rgba(0, 217, 163, 0.2);
  --text-badge: #ffffff;
  --text-primary: #ffffff;
  --text-secondary: #b0b0b0;
  --text-muted: #666666;
  --border-color: rgba(255, 255, 255, 0.08);
  --border-input: rgba(255, 255, 255, 0.1);
  --shadow: rgba(0, 0, 0, 0.4);
}

/* Light mode CSS variables */
[data-theme="light"] {
  --bg-primary: #f5f7fa;
  --bg-secondary: rgba(255, 255, 255, 0.9);
  --bg-card: rgba(255, 255, 255, 0.9);
  --bg-input: #ffffff;
  --bg-badge: rgba(0, 217, 163, 0.25);
  --border-badge: rgba(0, 217, 163, 0.4);
  --text-badge: #00856b;
  --text-primary: #2c3e50;
  --text-secondary: #6c757d;
  --text-muted: #adb5bd;
  --border-color: rgba(0, 0, 0, 0.06);
  --border-input: #e0e0e0;
  --shadow: rgba(0, 0, 0, 0.08);
}

body {
  margin: 0;
  padding: 0;
  font-family: 'Inter', -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
  background: var(--bg-primary);
  color: var(--text-primary);
  transition: background 0.3s ease, color 0.3s ease;
}

#app {
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  min-height: 100vh;
}
</style>

