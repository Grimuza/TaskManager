import { ref, watch, onMounted } from 'vue';

const THEME_KEY = 'task-manager-theme';

// Shared state
const isDark = ref(true);
let initialized = false;

// Apply theme function
function applyTheme(dark: boolean) {
  const html = document.documentElement;
  const body = document.body;
  const theme = dark ? 'dark' : 'light';
  
  html.setAttribute('data-theme', theme);
  body.setAttribute('data-theme', theme);
  
  if (dark) {
    html.classList.add('dark');
    html.classList.remove('light');
    body.classList.add('dark');
    body.classList.remove('light');
  } else {
    html.classList.add('light');
    html.classList.remove('dark');
    body.classList.add('light');
    body.classList.remove('dark');
  }
  
  localStorage.setItem(THEME_KEY, theme);
  console.log('Theme applied:', theme);
}

/**
 * Composable for theme management (dark/light mode)
 */
export function useTheme() {
  // Initialize once
  if (!initialized) {
    const savedTheme = localStorage.getItem(THEME_KEY);
    if (savedTheme) {
      isDark.value = savedTheme === 'dark';
    }
    applyTheme(isDark.value);
    initialized = true;

    // Watch for changes
    watch(isDark, (newValue) => {
      applyTheme(newValue);
    });
  }

  const toggleTheme = () => {
    console.log('Toggle clicked! Current:', isDark.value);
    isDark.value = !isDark.value;
    console.log('New value:', isDark.value);
  };

  return {
    isDark,
    toggleTheme,
  };
}

