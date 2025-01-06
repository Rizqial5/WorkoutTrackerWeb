import { AppBar, Toolbar, IconButton, Typography, Box } from "@mui/material"
import MenuIcon from '@mui/icons-material/Menu';
import theme from "../theme/them";

export default function TopBar({open, toggleDrawer, titleText}) {
    return <AppBar
        position='fixed'
        sx={{
            width: open ? `calc(100% - ${240}px)` : '100%',
            ml: open ? `${240}px` : 0,
            transition: theme.transitions.create(['margin', 'width'], {
                easing: theme.transitions.easing.sharp,
                duration: theme.transitions.duration.leavingScreen
            }),
        }}
    >
        <Toolbar>
            <IconButton
                color='inherit'
                aria-label='open drawer'
                onClick={toggleDrawer}
                edge='start'
                sx={{ mr: 2, ...(open && { display: 'none' }) }}
            >
                <MenuIcon />
            </IconButton>
            <Typography variant='h6' noWrap component='div'>
                {titleText}
            </Typography>
            <Box sx={{ flexGrow: 1 }} />

            <Box sx={{
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'center',
            color: '#1976d2',
        }}>
            <Typography color="white">
                Selamat Datang, <strong>Username</strong>
            </Typography>
                
            </Box>
        </Toolbar>
    </AppBar>
}