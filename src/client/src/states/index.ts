import {routerReducer} from 'react-router-redux';
import {combineReducers} from 'redux';

import {LOGOUT_SUCCESSFUL} from '../actions/account/authentication/EventTypes';

export const logoutReducer = (reducers: any): any => {
  return (state, action) => {
    if (action.type === LOGOUT_SUCCESSFUL) {
      return reducers(undefined, action);
    }
    return reducers(state, action);
  };
};

export default logoutReducer(
  combineReducers({
    routing: routerReducer,
    // ...account,
    // ...encoding,
    // ...player,
    // ...analytics,
    // ...dashboard,
    // ...misc,
    // ...contact,
    // ...gettingStarted,
    // featureDiscovery
  })
);
