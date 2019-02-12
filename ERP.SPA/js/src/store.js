import { createStore, applyMiddleware, compose } from "redux";

import { loadUser } from "redux-oidc";
import reducers from "reducers";
import userManager from "userManager";

const loggerMiddleware = store => next => action => {
  console.log("Action type:", action.type);
  console.log("Action payload:", action.payload);
  console.log("State before:", store.getState());
  next(action);
  console.log("State after:", store.getState());
};

const initialState = {};

const createStoreWithMiddleware = compose(
  applyMiddleware(loggerMiddleware)
)(createStore);

const store = createStoreWithMiddleware(reducers, initialState, window.__REDUX_DEVTOOLS_EXTENSION__ && window.__REDUX_DEVTOOLS_EXTENSION__());
loadUser(store, userManager);

export default store;