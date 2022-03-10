import './App.css';
import { BrowserRouter as Router, Switch, Route, useHistory } from 'react-router-dom';
import { useState, useEffect, useMemo } from 'react';
import { createBrowserHistory } from "history";

import ProtectedRoute from './components/AcountManagment/ProtectedRoute';
import { UserContext } from './components/AcountManagment/UserContext';

import Home from './components/Home/Home'
import Login from './components/AcountManagment/Login/Login';
import Register from './components/AcountManagment/Register/Register'
import Logout from './components/AcountManagment/Logout';
import Privacy from './components/PrivacyPolicy/Privacy';
import Header from './components/Header/Header';
import Footer from './components/Footer/Footer';

import Search from './components/SearchMovie/Search';

function App() {
  const [appUser, setAppUser] = useState({});

  const jwt = localStorage.getItem("jwt");
  const isAuthenticated = Object.keys(appUser ? appUser : {}).length !== 0;
  const value = useMemo(() => ({ appUser, setAppUser }), [appUser, setAppUser]);

  const history = useHistory();
  const browserHistory = createBrowserHistory();

  useEffect(() => {
    const userId = localStorage.getItem("userId");
    if (userId && jwt) {
      const url = process.env.REACT_APP_BASEURL + 'api/Users/getUser/id?userId=' + userId;
      fetch(url, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          'Authorization': 'Bearer ' + jwt
        },
      }
      ).then(r => {
        if (!r.ok) {
          localStorage.clear();
        }
        return r.json()
      })
        .then(result => {
          setAppUser(result)
        });
    }
  }, []);

  return (
    <UserContext.Provider value={value}>
      <div>
        <Router history={browserHistory}>
          <main className="App">
          <Header />
            <Switch>
              <Route path='/' exact render={Home} />
              <Route path='/Login' component={Login} />
              <Route path='/Register' component={Register} />
              <Route path='/Logout' component={Logout} />
              <Route path='/Error' component={Error} />
              <Route path='/Privacy' component={Privacy} />

              <ProtectedRoute path='/Search' component={Search} auth={isAuthenticated} />
            </Switch>
          </main>
          <Footer />
        </Router>
      </div>
    </UserContext.Provider >
  );
}

export default App;
