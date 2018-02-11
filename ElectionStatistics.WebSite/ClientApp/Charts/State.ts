import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction, LazyItems } from '../ApplicationState';
import { ElectionsState, electionsActionCreators, Election } from '../Elections/State';

export interface ChartsState {
    selectedElectionId?: number;
    showChart?: boolean;
}

export interface ChartsPageRouteProps{
    electionId?: number,
    showChart?: boolean
}

interface SelectElectionAction {
    type: 'SELECT_ELECTION';
    electionId: number;
}

interface LoadParametersAction {
    type: 'LOAD_PARAMETERS';
    stateToLoad: ChartsState;
}

type KnownAction = SelectElectionAction | LoadParametersAction;

export const chartsActionCreators = {
    ...electionsActionCreators,
    selectElection: (electionId: number) => <SelectElectionAction>{ type: 'SELECT_ELECTION', electionId: electionId },
    loadParameters: (routeProps: ChartsPageRouteProps) => <LoadParametersAction>{ 
        type: 'LOAD_PARAMETERS', 
        stateToLoad: {
            selectedElectionId: routeProps.electionId,
            showChart: routeProps.showChart
        } 
    }
};

export const chartsInitialState: ChartsState = { };

export const chartsReducer: Reducer<ChartsState> = (state: ChartsState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'SELECT_ELECTION':
            return {
                ...state,
                selectedElectionId: action.electionId
            };
        case 'LOAD_PARAMETERS':
            return {
                ...state,
                ...action.stateToLoad
            };
        default:
            const exhaustiveCheck: never = action;
    }

    return state || chartsInitialState;
};
