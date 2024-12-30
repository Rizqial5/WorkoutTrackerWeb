import { createTheme } from '@mui/material/styles';

const theme = createTheme({
  palette: {
    primary: {
      main: '#2196f3', // Blue
      light: '#64b5f6',
      dark: '#1976d2',
    },
    secondary: {
      main: '#ffffff', // White
    },
    background: {
      default: '#ffffff', // White
      paper: '#ffffff', // White
    },
    text: {
      primary: '#2196f3', // Blue
      secondary: '#64b5f6', // Light Blue
    },
  },
  typography: {
    fontFamily: 'Roboto, Arial, sans-serif',
    h6: {
      fontWeight: 600,
    },
  },
  components: {
    MuiListItemButton: {
      styleOverrides: {
        root: {
          '&:hover': {
            backgroundColor: 'rgba(33, 150, 243, 0.08)', // Light blue with opacity
          },
        },
      },
    },
    MuiListItemIcon: {
      styleOverrides: {
        root: {
          color: 'inherit',
        },
      },
    },
  },
});

export default theme;

