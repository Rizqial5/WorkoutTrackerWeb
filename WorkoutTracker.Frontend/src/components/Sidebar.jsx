import React from 'react';
import {
  Drawer,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Toolbar,
  Typography,
  useTheme,
} from '@mui/material';
import {
  FitnessCenter,
  CalendarToday,
  Timeline,
  Settings,
  Home,
} from '@mui/icons-material';


const drawerWidth = 240;

const menuItems = [
  { text: 'Dashboard', icon: Home, path: '/' },
  { text: 'Workouts', icon: FitnessCenter, path: '/workouts' },
  { text: 'Schedule', icon: CalendarToday, path: '/schedule' },
  { text: 'Progress', icon: Timeline, path: '/progress' },
  { text: 'Settings', icon: Settings, path: '/settings' },
];

export default function Sidebar() {
  const themeSide = useTheme();

  return (
    <Drawer
      sx={{
        width: drawerWidth,
        flexShrink: 0,
        '& .MuiDrawer-paper': {
          width: drawerWidth,
          boxSizing: 'border-box',
          backgroundColor: themeSide.palette.background.default,
          color: themeSide.palette.primary.main,
        },
      }}
      variant="permanent"
      anchor="left"
    >
      <Toolbar>
        <Typography variant="h6" noWrap component="div" color="primary">
          Workout Planner
        </Typography>
      </Toolbar>
      <List>
        {menuItems.map((item) => (
          <ListItem key={item.text} disablePadding>
            <ListItemButton>
              <ListItemIcon>
                <item.icon />
              </ListItemIcon>
              <ListItemText primary={item.text} />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
    </Drawer>
  );
}

