import { Modal, Table } from '@mantine/core';

const Scoreboard = props => {
    const { value, ...rest } = props;

    const rows = value.sort((a, b) => b.round - a.round).slice(0, 10);

    return (
        <Modal {...rest} title="Scoreboard" size="xl" centered withinPortal>
            <Table striped highlightOnHover>
                <thead>
                    <tr>
                        <th>Round</th>
                        <th>Player Choice</th>
                        <th>Computer Choice</th>
                        <th>Result</th>
                        <th>Player Score</th>
                        <th>Computer Score</th>
                    </tr>
                </thead>
                <tbody>
                    {rows.map(
                        ({
                            round,
                            playerChoice,
                            computerChoice,
                            result,
                            playerScore,
                            computerScore
                        }) => (
                            <tr key={round}>
                                <td>{round}</td>
                                <td>{playerChoice}</td>
                                <td>{computerChoice}</td>
                                <td>{result}</td>
                                <td>{playerScore}</td>
                                <td>{computerScore}</td>
                            </tr>
                        )
                    )}
                </tbody>
            </Table>
        </Modal>
    );
};

export default Scoreboard;
