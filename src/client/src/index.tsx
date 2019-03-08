// import {createHistory} from 'history';
import * as React from 'react';
import {render} from 'react-dom';
import {Provider} from 'react-redux';
// import {Route/*, Router, useRouterHistory*/} from 'react-router';
// import {BrowserRouter/*, routerMiddleware*/} from 'react-router-redux';
import {
  BrowserRouter as Router,
  Route,
  Switch
  // etc.
} from 'react-router-dom'
import {applyMiddleware, createStore} from 'redux';
import thunk from 'redux-thunk';
import Home from './components/App';
import Test from './components/Test';
import registerServiceWorker from './registerServiceWorker';
import stateReducer from './states';
import './styles/main.scss';
import NotFoundView from './views/NotFoundView';
// import {scrollTop} from './utils/DomUtils';

// const browserHistory = useRouterHistory(createHistory)({
//   basename: PUBLIC_URL
// });
const middleWare = [thunk/*, routerMiddleware(browserHistory)*/];

const store = createStore(stateReducer, applyMiddleware(...middleWare));

// const history = syncHistoryWithStore(browserHistory, store);

// const initPromise = store.dispatch(tryLoginAndInitPortal());

const appContainer = document.getElementById('root');

// render(<LoadingView />, appContainer);
// v4
const App = () => (
  <Switch>
    {/* {unauthenticated} */}
    <Route exact path='/' component={Home} />
    <Route path='/about' component={Test} />
    <Route path="*" component={NotFoundView} /> 
  </Switch>
)
// initPromise.then(() => {
  render(
    <Provider store={store}>
      {/* <Router onUpdate={() => scrollTop('#content-area')}> */}
      <Router>
        <Route path="/" component={App} />
      </Router>
    </Provider>,
    appContainer
  );
// });

registerServiceWorker();
