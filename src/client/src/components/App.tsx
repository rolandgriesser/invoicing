import * as React from 'react';
import '../styles/App.css';

import logo from '../media/logo.svg';
import { Link } from 'react-router-dom';

class App extends React.Component {
  public render() {
    return (
      <div className="App">
        <header className="App-header">
          <img src={logo} className="App-logo" alt="logo" />
          <h1 className="App-title">Welcome to React</h1>
        </header>
        <p className="App-intro">
          To get started, edit <code>src/App.tsx</code> and save to reload.
        </p>
        <Link to="/about">About</Link>
        <Link to="/asf">nf</Link>
      </div>
    );
  }
}

export default App;
