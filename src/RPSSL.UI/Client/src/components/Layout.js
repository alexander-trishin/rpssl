import {MantineProvider} from '@mantine/core';

const Layout = props => {
    const { children } = props;

    return (
        <MantineProvider theme={{ colorScheme: 'dark' }} withGlobalStyles withNormalizeCSS>
            {children}
        </MantineProvider>
    );
};

export default Layout;
