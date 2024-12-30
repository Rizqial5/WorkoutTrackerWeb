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
  Button,
  IconButton
} from '@mui/material';
import {
  FitnessCenter,
  CalendarToday,
  Timeline,
  Settings,
  Home,
  ChevronLeft,
} from '@mui/icons-material';
import { logout } from '../services/authService';
import { useNavigate } from 'react-router-dom';


const drawerWidth = 240;

const menuItems = [
  { text: 'Dashboard', icon: Home, path: '/dashboard' },
  { text: 'Workouts', icon: FitnessCenter, path: '/workouts' },
  { text: 'Schedule', icon: CalendarToday, path: '/schedule' },
  { text: 'Progress', icon: Timeline, path: '/progress' },
  { text: 'Settings', icon: Settings, path: '/settings' },
];

export default function Sidebar({openBar, toggleDrawer}) {
  const themeSide = useTheme();
  const navigate = useNavigate();
  const handleLogout =()=> {
    logout()
    console.log("logout complete")
    console.log(localStorage.getItem('token'))
    navigate('/login')
  }

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
      variant="persistent"
      anchor="left"
      open= {openBar}
    >
        <Toolbar sx ={{display: 'flex', alignItems: 'center', justifyContent: 'flex-end', px:[1]}}>
          <Typography variant="h6" noWrap component="div" color="primary" sx={{flexGrow: 1}}>
              Workout Planner
          </Typography>
          <IconButton onClick={toggleDrawer}>
            <ChevronLeft/>
          </IconButton>
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
        <Button onClick={handleLogout} variant='contained' sx={{
            marginTop: 45,
            marginLeft: 9,
            width: 100,
            backgroundColor: '#ed3e32'
            }}>Logout</Button>
    </Drawer>
  );
}
