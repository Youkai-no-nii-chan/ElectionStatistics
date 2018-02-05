import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';

import Select from 'react-select';

export class ChartsPage extends React.Component<RouteComponentProps<{}>, {}> {
	state = {
		selectedOption: '',
	}
	handleChange = (selectedOption: any) => {
        this.setState({ selectedOption });
        console.log(`Selected: ${selectedOption.label}`);
	}
    public render() {
        const { selectedOption } = this.state;

        return (
            <Select
                name="form-field-name"
                value={selectedOption}
                onChange={this.handleChange}
                options={[
                    { value: 'one', label: 'One' },
                    { value: 'two', label: 'Two' },
                ]}
            />
        );
    }
}
