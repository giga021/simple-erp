import { combineReducers } from 'redux';
import { reducer as oidcReducer } from 'redux-oidc';
import subscriptionsReducer from './subscriptions';

const reducers = combineReducers(
  {
    oidc: oidcReducer,
    subscriptions: subscriptionsReducer
  }
);

export default reducers;