<template>
  <button 
    @click="toggleTheme" 
    class="theme-toggle"
    :title="isDark ? 'Switch to light mode' : 'Switch to dark mode'"
  >
    <!-- Sun icon for dark mode -->
    <svg v-if="isDark" class="icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
      <circle cx="12" cy="12" r="5"/>
      <line x1="12" y1="1" x2="12" y2="3"/>
      <line x1="12" y1="21" x2="12" y2="23"/>
      <line x1="4.22" y1="4.22" x2="5.64" y2="5.64"/>
      <line x1="18.36" y1="18.36" x2="19.78" y2="19.78"/>
      <line x1="1" y1="12" x2="3" y2="12"/>
      <line x1="21" y1="12" x2="23" y2="12"/>
      <line x1="4.22" y1="19.78" x2="5.64" y2="18.36"/>
      <line x1="18.36" y1="5.64" x2="19.78" y2="4.22"/>
    </svg>
    
    <!-- Moon icon for light mode -->
    <svg v-else class="icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
      <path d="M21 12.79A9 9 0 1 1 11.21 3 7 7 0 0 0 21 12.79z"/>
    </svg>
  </button>
</template>

<script setup lang="ts">
import { useTheme } from '../composables/useTheme';

const { isDark, toggleTheme } = useTheme();
</script>

<style scoped>
.theme-toggle {
  position: fixed;
  top: 20px;
  right: 20px;
  width: 48px;
  height: 48px;
  border-radius: 50%;
  border: 1px solid rgba(255, 255, 255, 0.1);
  background: rgba(25, 25, 25, 0.6);
  backdrop-filter: blur(10px);
  cursor: pointer;
  transition: all 0.3s ease;
  z-index: 1000;
  display: flex;
  align-items: center;
  justify-content: center;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
  color: #888888;
}

.theme-toggle:hover {
  transform: scale(1.05);
  border-color: rgba(0, 217, 163, 0.4);
  background: rgba(30, 30, 30, 0.7);
  color: #00d9a3;
}

.theme-toggle:active {
  transform: scale(0.95);
}

.icon {
  width: 22px;
  height: 22px;
  transition: all 0.3s ease;
}

.theme-toggle:hover .icon {
  transform: rotate(20deg);
}

/* Light mode styles */
:global([data-theme="light"]) .theme-toggle {
  background: rgba(255, 255, 255, 0.8) !important;
  border-color: rgba(0, 0, 0, 0.1) !important;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.08) !important;
  color: #666666 !important;
}

:global([data-theme="light"]) .theme-toggle:hover {
  background: rgba(255, 255, 255, 0.95) !important;
  border-color: rgba(0, 217, 163, 0.3) !important;
  color: #00d9a3 !important;
}
</style>

