import * as React from 'react';
import { RouteComponentProps, Link } from 'react-router-dom';

import Select, { Option } from 'react-select';
import { ChartsState, chartsActionCreators, ChartsPageRouteProps } from './State';
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
    RouteComponentProps<ChartsPageRouteProps>

class ChartsPage extends React.Component<ChartsPageProps, {}> {
    public componentWillMount() {
        this.props.loadParameters(this.props.match.params);
        this.props.requestElections();
    }

    public render() {
        return (
            <div>
                {this.renderSelect()}
                {this.renderButton()}
            </div>
        );
    }

    private renderSelect() {
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

    private handleChange = (selectedOption: any) => {
        this.props.selectElection(selectedOption.value)
    }

    private renderButton() {
        let className = "btn btn-primary"
        let disabled = false;
        if (this.props.charts.selectedElectionId == null) {
            className += "btn btn-primary disabled";
            disabled = true;
        }

        const queryParams: ChartsPageRouteProps = {
            electionId: this.props.charts.selectedElectionId,
            showChart: true
        }

        return (
            <Link 
                className={className}
                disabled={disabled}
                to={`/charts?${this.serializeToQueryString(queryParams)}`}>
                Построить график
            </Link>
        );
    }

    private serializeToQueryString(obj: any, prefix?: string) {
        const str = new Array<string>();
        for(const property in obj) {
          if (obj.hasOwnProperty(property)) {
            const key = prefix ? prefix + "[" + property + "]" : property;
            const value = obj[property];
            str.push((value !== null && typeof value === "object") ?
              this.serializeToQueryString(value, key) :
              encodeURIComponent(key) + "=" + encodeURIComponent(value));
          }
        }
        return str.join("&");
      }
}

export default connect(
    (state: ApplicationState) => state as ChartsPageState,
    chartsActionCreators                 // Selects which action creators are merged into the component's props
)(ChartsPage) as typeof ChartsPage;
