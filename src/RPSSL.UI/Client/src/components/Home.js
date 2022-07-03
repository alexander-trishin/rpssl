import {
    Box,
    Button,
    Center,
    createStyles,
    Group,
    Paper,
    Select,
    SimpleGrid,
    Stack,
    Text,
    Title
} from '@mantine/core';
import { useEffect, useState } from 'react';

const useHomeStyles = createStyles(theme => ({
    root: {
        position: 'fixed',
        inset: 0
    },

    round: {
        ...theme.headings.sizes.h3
    },

    roundResult: {
        ...theme.headings.sizes.h4
    },

    grid: {
        marginTop: theme.spacing.xl * 2,
        marginBottom: theme.spacing.xl * 2
    },

    card: {
        height: '100%'
    }
}));

const Home = () => {
    const { classes, theme } = useHomeStyles();

    const [round, setRound] = useState(0);
    const [roundResult, setRoundResult] = useState('vs');
    const [roundResultColor, setRoundResultColor] = useState(theme.primaryColor);

    const [choices, setChoices] = useState([]);

    const [playerChoice, setPlayerChoice] = useState('');
    const [computerChoice, setComputerChoice] = useState('Waiting for your selection...');

    const [playerScore, setPlayerScore] = useState(0);
    const [computerScore, setComputerScore] = useState(0);

    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        let cancel = false;

        const initialize = async () => {
            const response = await fetch('api/choices');
            const data = await response.json();

            if (!cancel) {
                setChoices(data.map(({ id, name }) => ({ value: id.toString(), label: name })));

                setIsLoading(false);
            }
        };

        initialize();

        return () => {
            cancel = true;
        };
    }, []);

    const handlePlay = async () => {
        setIsLoading(true);

        try {
            const response = await fetch('api/play', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    player: parseInt(playerChoice)
                })
            });

            const { computer, results } = await response.json();

            switch (results) {
                case 'tie': {
                    setRoundResult("It's a tie!");
                    setRoundResultColor('gray');
                    break;
                }
                case 'win': {
                    setRoundResult('You won!');
                    setRoundResultColor('green');
                    setPlayerScore(prevScore => prevScore + 1);
                    break;
                }
                case 'lose': {
                    setRoundResult('You lost!');
                    setRoundResultColor('red');
                    setComputerScore(prevScore => prevScore + 1);
                    break;
                }
                default:
                    throw new Error(`Unknown result: ${results}`);
            }

            setComputerChoice(choices.find(choice => choice.value === computer.toString()).label);
            setRound(prevRound => prevRound + 1);
        } finally {
            setIsLoading(false);
        }
    };

    const handleReset = () => {
        setRound(0);
        setRoundResult('vs');
        setRoundResultColor(theme.primaryColor);

        setPlayerChoice('');
        setComputerChoice('Waiting for your selection...');

        setPlayerScore(0);
        setComputerScore(0);

        setIsLoading(false);
    };

    return (
        <Center className={classes.root}>
            <Stack>
                <Title order={1} color="#fff" align="center" mb={0}>
                    Rock, Paper, Scissors, Spock, Lizard
                </Title>

                <Text
                    className={classes.round}
                    component="h2"
                    color={theme.primaryColor}
                    align="center"
                    mt={0}
                    mb="xl"
                >
                    {round ? `Round: ${round}` : 'No rounds yet!'}
                </Text>

                <Box className={classes.grid}>
                    <SimpleGrid cols={3}>
                        <Stack justify="center">
                            <Paper
                                className={classes.card}
                                shadow="xl"
                                radius="md"
                                p="xl"
                                withBorder
                            >
                                <Stack>
                                    <Title order={5} align="center">
                                        Player
                                    </Title>
                                    <Select
                                        placeholder="Choose your move"
                                        disabled={isLoading}
                                        data={choices}
                                        value={playerChoice}
                                        onChange={setPlayerChoice}
                                        my="xl"
                                    />
                                </Stack>
                            </Paper>
                        </Stack>

                        <Center>
                            <Text
                                className={classes.roundResult}
                                component="h4"
                                color={roundResultColor}
                                align="center"
                            >
                                {roundResult}
                            </Text>
                        </Center>

                        <Stack>
                            <Paper
                                className={classes.card}
                                shadow="xl"
                                radius="md"
                                p="xl"
                                withBorder
                            >
                                <Stack>
                                    <Title order={5} align="center">
                                        Computer
                                    </Title>
                                    <Text align="center" my="xl">
                                        {computerChoice}
                                    </Text>
                                </Stack>
                            </Paper>
                        </Stack>
                    </SimpleGrid>

                    <SimpleGrid cols={3} mt="xl">
                        <Title order={3} align="center">
                            {playerScore}
                        </Title>
                        <Box />
                        <Title order={3} align="center">
                            {computerScore}
                        </Title>
                    </SimpleGrid>
                </Box>

                <Center>
                    <Group spacing="xl">
                        <Button disabled={!playerChoice} loading={isLoading} onClick={handlePlay}>
                            Play
                        </Button>
                        {round && (
                            <Button variant="outline" disabled={isLoading} onClick={handleReset}>
                                Reset
                            </Button>
                        )}
                    </Group>
                </Center>
            </Stack>
        </Center>
    );
};

export default Home;
