import TopBar from './TopBar.jsx'
import Sidebar from './Sidebar.jsx'
import React, { useState } from 'react'
import theme from '../theme/them.js'
import { Box, Toolbar} from '@mui/material'
import WorkoutPlans from './WorkoutPlans.jsx'

export default function Dashboard({open}){

    return(
        <Box
            component='main'
            sx={{
                flexGrow: 1,
                p: 3,
                backgroundColor: 'background.default',
                transition: theme.transitions.create('margin', {
                    easing: theme.transitions.easing.sharp,
                    duration: theme.transitions.duration.leavingScreen,
                }),
                marginLeft: `-${240}px`,
                    ...(open && {
                    transition: theme.transitions.create('margin', {
                        easing: theme.transitions.easing.easeOut,
                        duration: theme.transitions.duration.enteringScreen,
                    }),
                    marginLeft: 0,
                }),
            }}
        >
            <Toolbar/>
            <WorkoutPlans/>
        </Box>
    )
} 