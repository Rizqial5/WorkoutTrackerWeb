import { useEffect, useState } from "react";
import { getExerciseSets, addExerciseSet } from "../services/exerciseSetService";
import { 
    Box, 
    Table, 
    TableBody, 
    TableCell, 
    TableContainer, 
    TableHead, 
    TableRow, 
    Toolbar, 
    Button, Dialog, DialogContent, DialogContentText, Stack, Snackbar, Alert, CircularProgress,DialogTitle, TextField,DialogActions} from '@mui/material';
import Paper from '@mui/material/Paper';
import theme from '../theme/them.js';

export default function ExerciseSets({open}) {
    const [exerciseSetsData, setExerciseSetsData] = useState([])
    const [loading, setLoading] =  useState(true)
    const [error, setError] = useState('')

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
    
    useEffect(() => {
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
        <ModalForm onSubmitSuccess={fetchData}/>
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

const ModalForm = ({onSubmitSuccess}) => {
    const [open, setOpen] = useState(false);
    const [loading, setLoading] = useState(false);
    const [alert, setAlert] = useState({ open: false, message: '', severity: 'success' });

    const handleOpen = () => setOpen(true);
    const handleClose = () => setOpen(false);

    const handleSubmit = async (event) => {
        event.preventDefault();
        const formData = new FormData(event.target);
        const data = {
            exerciseSetName: formData.get('exerciseSetName'),
            repetitions: formData.get('repetitions'),
            set: formData.get('set'),
            weight: formData.get('weight'),
            exerciseId: formData.get('exerciseId')
        }

        try {
            setLoading(true);
            await addExerciseSet(data);
            
        } catch (error) {
            setAlert({ open: true, message: error, severity: 'error' });
        } finally {
            setLoading(false);
            setAlert({ open: true, message: 'Exercise Set Added Successfully', severity: 'success' });
            handleClose();
            event.target.reset();
            onSubmitSuccess();
        }
    }

    const handleCloseAlert = () => setAlert({ ...alert, open: false});

    return(
        <div>
        <Button sx={{mt:2}} variant='contained' color="primary" onClick={handleOpen}>Add Exercise Set</Button>
        <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
            <DialogTitle>Add Exercise Set</DialogTitle>
            <form onSubmit={handleSubmit}>
                <DialogContent>
                    <DialogContentText>
                        Fill Exercise Data Form
                    </DialogContentText>
                    <Stack  spacing={2} sx={{mt: 2 }}> 
                        <TextField
                            required
                            fullWidth
                            autoFocus
                            id="exerciseSetName"
                            type="text"
                            name="exerciseSetName"
                            label="Exercise Name"
                        />
                        <TextField
                            required
                            fullWidth
                            id="repetitions"
                            type="number"
                            name="repetitions"
                            label="Repetitions"
                        />
                        <TextField
                            required
                            fullWidth
                            type="number"
                            id="set"
                            name="set"
                            label="Sets"
                        />
                        <TextField
                            required
                            fullWidth
                            type="text"
                            id="weight"
                            name="weight"
                            label="Weight"
                        />
                        <TextField
                            required
                            fullWidth
                            type="number"
                            id="exerciseId"
                            name="exerciseId"
                            label="Exercise Id"
                        />
                    </Stack>
                </DialogContent>
                <DialogActions>
                    <Button onClick={handleClose}>Cancel</Button>
                    <Button 
                    type="submit" 
                    color="primary" 
                    disabled={loading}
                    startIcon={loading ? <CircularProgress size={20} /> : null}
                    >
                        {loading ? 'Adding...' : 'Add'}
                    </Button>
                </DialogActions>
            </form>   
        </Dialog>
        <Snackbar
            open={alert.open}
            autoHideDuration={6000}
            onClose={handleCloseAlert}
            anchorOrigin={{ vertical: 'top', horizontal: 'right' }}>
                <Alert
                    onClose={handleCloseAlert}
                    severity={alert.severity}
                    sx={{ width: '100%' }}
                > 
                {alert.message}
                </Alert>

        </Snackbar>

        </div>
    )
}