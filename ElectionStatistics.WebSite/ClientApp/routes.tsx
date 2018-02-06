import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './Layout/Component';
import { AboutPage } from './About/Page';
import ChartsPage from './Charts/Page';
import FetchData from './components/FetchData';

export const routes = 
    <Layout>
        <Route exact path='/' component={ AboutPage } />
        <Route path='/charts' component={ ChartsPage } />
        <Route path='/fetchdata/:startDateIndex?' component={ FetchData } />
    </Layout>;
