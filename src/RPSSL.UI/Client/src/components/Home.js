import {
    Box,
    Button,
    Center,
    createStyles,
    Group,
    Loader,
    Paper,
    SegmentedControl,
    SimpleGrid,
    Stack,
    Text,
    Title
} from '@mantine/core';
import { useEffect, useRef, useState } from 'react';
import Scoreboard from './Scoreboard';

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
        height: '100%',
        minWidth: 250
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

    const getChoiceLabelByValue = value => {
        return choices.find(choice => choice.value === value.toString())?.label;
    };

    const [showScoreboard, setShowScoreboard] = useState(false);
    const [scoreboard, setScoreboard] = useState([]);

    const toggleScoreboard = () => setShowScoreboard(prevShow => !prevShow);

    const scoreRef = useRef({});
    scoreRef.current = {
        round,
        playerChoice: playerChoice && getChoiceLabelByValue(playerChoice),
        computerChoice,
        result: roundResult,
        playerScore,
        computerScore
    };

    useEffect(() => {
        let cancel = false;

        const initialize = async () => {
            const response = await fetch('api/choices');
            const data = await response.json();

            if (!cancel) {
                const items = data.map(({ id, name }) => ({ value: id.toString(), label: name }));

                setChoices(items);
                setPlayerChoice(items[0].value);

                setIsLoading(false);
            }
        };

        initialize();

        return () => {
            cancel = true;
        };
    }, []);

    useEffect(() => {
        if (round > 0) {
            setScoreboard(prevScoreboard => [...prevScoreboard, scoreRef.current]);
        }
    }, [round]);

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

            setComputerChoice(getChoiceLabelByValue(computer));
            setRound(prevRound => prevRound + 1);
        } finally {
            setIsLoading(false);
        }
    };

    const handleReset = () => {
        setRound(0);
        setRoundResult('vs');
        setRoundResultColor(theme.primaryColor);

        setPlayerChoice(choices[0].value);
        setComputerChoice('Waiting for your selection...');

        setPlayerScore(0);
        setComputerScore(0);

        setIsLoading(false);

        setScoreboard([]);
    };

    return (
        <Center className={classes.root}>
            <Stack>
                <Title order={1} align="center" mb={0}>
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
                                <Stack align="center">
                                    <Title order={5} align="center">
                                        Player
                                    </Title>
                                    {choices.length > 0 ? (
                                        <SegmentedControl
                                            color={theme.primaryColor}
                                            orientation="vertical"
                                            disabled={isLoading}
                                            data={choices}
                                            value={playerChoice}
                                            onChange={setPlayerChoice}
                                            transitionDuration={300}
                                            my="xl"
                                        />
                                    ) : (
                                        <Loader size="xl" />
                                    )}
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
                        <Button
                            size="md"
                            disabled={!playerChoice}
                            loading={isLoading}
                            onClick={handlePlay}
                        >
                            Play
                        </Button>
                        {round && (
                            <>
                                <Button
                                    size="md"
                                    variant="light"
                                    disabled={isLoading}
                                    onClick={() => toggleScoreboard()}
                                >
                                    Scoreboard
                                </Button>
                                <Button
                                    size="md"
                                    variant="outline"
                                    disabled={isLoading}
                                    onClick={handleReset}
                                >
                                    Reset
                                </Button>
                            </>
                        )}
                    </Group>
                </Center>
            </Stack>

            <Scoreboard opened={showScoreboard} onClose={toggleScoreboard} value={scoreboard} />
        </Center>
    );
};

export default Home;
