import {Route, Routes} from 'react-router-dom';
import AppRoutes from './AppRoutes';
import Layout from './components/Layout';

const App = () => {
    return (
        <Layout>
            <Routes>
                {AppRoutes.map((route, index) => {
                    const { element, ...rest } = route;

                    return <Route key={index} {...rest} element={element}/>;
                })}
            </Routes>
        </Layout>
    );
};

export default App;
