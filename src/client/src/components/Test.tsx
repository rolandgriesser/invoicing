import * as React from 'react';
import '../styles/App.css';

import logo from '../media/logo.svg';
import { Link } from 'react-router-dom';

class Test extends React.Component {
  public render() {
    return (
      <div className="App">
        <header className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
          <h1 className="App-title">Welcome to React</h1>
        </header>
        <p className="App-intro">
          blablablalbsa
      <Link to="/asf">nf</Link>
        </p>
      </div>
    );
  }
}

export default Test;
