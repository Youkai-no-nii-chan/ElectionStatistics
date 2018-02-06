import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';

import Select, { Option } from 'react-select';
import { ChartsState, chartsActionCreators } from './State';
import { ElectionsState } from '../Elections/State';
import { ApplicationState } from '../ApplicationState';
import { connect } from 'react-redux';


interface ChartsPageState {
    elections: ElectionsState,
    charts: ChartsState
}

// At runtime, Redux will merge together...
type ChartsPageProps = 
    ChartsPageState & 
    typeof chartsActionCreators &
    RouteComponentProps<{}>

class ChartsPage extends React.Component<ChartsPageProps, {}> {
    componentWillMount() {
        this.props.requestElections();
    }

    handleChange = (selectedOption: any) => {
        this.props.selectElection(selectedOption.value)
    }

    public render() {
        if (!this.props.elections || !this.props.elections.isLoaded) {
            return (
                <Select
                    name="form-field-name"
                    isLoading={true}
                />);
        }
        else {
            const options = this.props.elections.items
                .map(election => ({ value: election.id as number, label: election.name }));

            return (
                <Select
                    name="form-field-name"
                    onChange={this.handleChange}
                    value={this.props.charts.selectedElectionId}
                    options={options}
                />
            );
        }
    }
}

export default connect(
    (state: ApplicationState) => state as ChartsPageState,
    chartsActionCreators                 // Selects which action creators are merged into the component's props
)(ChartsPage) as typeof ChartsPage;
