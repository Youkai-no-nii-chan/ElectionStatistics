import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction, LazyItems } from '../ApplicationState';
import { ElectionsState, electionsActionCreators, Election } from '../Elections/State';

export interface ChartsState {
    selectedElectionId?: number;
}

interface SelectElectionAction {
    type: 'SELECT_ELECTION';
    electionId: number;
}

type KnownAction = SelectElectionAction;

export const chartsActionCreators = {
    ...electionsActionCreators,
    selectElection: (electionId: number) => <SelectElectionAction>{ type: 'SELECT_ELECTION', electionId: electionId }
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
    }

    return state || chartsInitialState;
};
