import React, {Component} from 'react';

interface Props {
  errorMessage?: string;
}

class NotFoundView extends Component<Props> {

  render() {
    return (
      <div>Not found
      </div>
    );
  }
}

export default NotFoundView;