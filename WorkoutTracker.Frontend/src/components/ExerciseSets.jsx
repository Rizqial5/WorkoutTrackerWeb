import { useEffect, useState } from "react";
import { getExerciseSets } from "../services/exerciseSetService";
import { Box, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Toolbar} from '@mui/material';
import Paper from '@mui/material/Paper';
import theme from '../theme/them.js';

export default function ExerciseSets({open}) {
    const [exerciseSetsData, setExerciseSetsData] = useState([])
    const [loading, setLoading] =  useState(true)
    const [error, setError] = useState('')

    useEffect(() => {
       const fetchData = async () => {
        try{
            const data = await getExerciseSets();
            setExerciseSetsData(data);
            setLoading(false);
        } catch(err){
            setError(err.message);
            setLoading(false);
        }
       };

       fetchData();
    }, []);

    if(loading)  return <p>Loading...</p>
    if(error) return <p>Error : {error}</p>

    return (
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
        {ShowDataTable(exerciseSetsData)}
        </Box>
    )
}

function ShowDataTable(exerciseSetsData) {
    return <TableContainer component={Paper}>
        <Table sx={{ minWidth: 650 }} aria-label="simple table">
            <TableHead>
                <TableRow>
                    <TableCell>Exercise</TableCell>
                    <TableCell>Reps</TableCell>
                    <TableCell>Sets</TableCell>
                    <TableCell>Weight</TableCell>
                </TableRow>
            </TableHead>
            <TableBody>
                {exerciseSetsData.map((es) => (
                    <TableRow
                        key={es.exerciseSetId}
                        sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                    >
                        <TableCell>{es.exerciseSetName}</TableCell>
                        <TableCell>{es.repetitions}</TableCell>
                        <TableCell>{es.set}</TableCell>
                        <TableCell>{es.weight}</TableCell>

                    </TableRow>
                ))}
            </TableBody>
        </Table>
    </TableContainer>;
}
