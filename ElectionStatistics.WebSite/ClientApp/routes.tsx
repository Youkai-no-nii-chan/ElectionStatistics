import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './Layout/Layout';
import { AboutPage } from './About/AboutPage';
import FetchData from './components/FetchData';

export const routes = 
    <Layout>
        <Route exact path='/' component={ AboutPage } />
        <Route path='/charts' component={ AboutPage } />
        <Route path='/fetchdata/:startDateIndex?' component={ FetchData } />
    </Layout>;
