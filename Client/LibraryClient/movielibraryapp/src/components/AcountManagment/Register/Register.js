import React, { useState } from "react";
import { Redirect } from "react-router-dom";
import "./Register.css";
import Footer from '../../Footer/Footer'
import { TextField } from "@mui/material";
import Button from "@mui/material/Button";
import InputAdornment from "@mui/material/InputAdornment";
import TitleIcon from "@mui/icons-material/Title";
import ShortTextIcon from "@mui/icons-material/ShortText";
import ConnectWithoutContactIcon from "@mui/icons-material/ConnectWithoutContact";
import DescriptionIcon from "@mui/icons-material/Description";
import IconButton from "@mui/material/IconButton";
import OutlinedInput from "@mui/material/OutlinedInput";
import InputLabel from "@mui/material/InputLabel";
import Visibility from "@mui/icons-material/Visibility";
import VisibilityOff from "@mui/icons-material/VisibilityOff";
import FormControl from "@mui/material/FormControl";
import MenuItem from "@mui/material/MenuItem";
import Select from "@mui/material/Select";

import ErrorNotification from "../../ErrorsManagment/ErrorNotification";

const Register = () => {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [gender, setGender] = useState('');
  const [email, setEmail] = useState("");
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setshowPassword] = useState(false);
  const [redirect, setRedirect] = useState(false);
  const [error, setError] = useState();
  const registerUrl = process.env.REACT_APP_BASEURL + 'api/Users/register';

  const handleChange = (e) => {
    setGender(e.target.value);
  };

  const handleClickShowPassword = () => {
    setshowPassword(!showPassword);
  };

  const handleMouseDownPassword = (event) => {
    event.preventDefault();
  };

  const submit = async (e) => {
    e.preventDefault();

    const formData = new FormData();
    formData.append("FirstName", firstName);
    formData.append("LastName", lastName);
    formData.append("Gender", gender);
    formData.append("Email", email);
    formData.append("UserName", username);
    formData.append("Password", password);


    await fetch(registerUrl, {
      method: "POST",
      body: formData,
    }).then(res => {
      if (!res.ok) {
        throw new Error('Register failed! Try again.')
      }
      setRedirect(true);
      return res.json();
    })
    .catch(err => {
      setError(err.message);
    })
  };
  if (redirect) {
    return <Redirect to="/Login" />;
  }
  return (
    <div className="Register">
      {error ? <ErrorNotification message={error} /> :
        <form onSubmit={(e) => submit(e)}>
          <h1 className="title__register">
            <ConnectWithoutContactIcon /> Please , fill your details to register.
          </h1>
          <div>
            <TextField
              className="textFieldTitle"
              label="First name"
              variant="filled"
              size="large"
              fullWidth
              onChange={(e) => setFirstName(e.target.value)}
              required
              multiline
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <TitleIcon />
                  </InputAdornment>
                ),
              }}
            />
          </div>
          <br />
          <div>
            <TextField
              label="Last name"

              variant="filled"
              size="large"
              fullWidth
              onChange={(e) => setLastName(e.target.value)}
              required
              multiline
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <DescriptionIcon />
                  </InputAdornment>
                ),
              }}
            />
          </div>
          <br />
          <div>
            <FormControl fullWidth>
              <InputLabel>Gender</InputLabel>
              <Select
                required
                value={gender}
                label="Gender"
                onChange={handleChange}
              >
                <MenuItem value="Male">Male</MenuItem>
                <MenuItem value="Female">Female</MenuItem>
              </Select>
            </FormControl>
          </div>
          <br />
          <div>
            <TextField
              type="email"
              label="Email"
              variant="filled"
              size="large"
              fullWidth
              onChange={(e) => setEmail(e.target.value)}
              required
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <ShortTextIcon />
                  </InputAdornment>
                ),
              }}
            />
          </div>
          <br />
          <div>
            <TextField
              className="username"
              label="Username"
              variant="filled"
              size="large"
              fullWidth
              onChange={(e) => setUsername(e.target.value)}
              required
              multiline
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <ShortTextIcon />
                  </InputAdornment>
                ),
              }}
            />
            <FormControl className="password" variant="filled">
              <InputLabel htmlFor="outlined-adornment-password">
                Password
              </InputLabel>
              <OutlinedInput
                className="passwordInput"
                type={showPassword ? "text" : "password"}
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                endAdornment={
                  <InputAdornment position="end">
                    <IconButton
                      aria-label="toggle password visibility"
                      onClick={handleClickShowPassword}
                      onMouseDown={handleMouseDownPassword}
                      edge="end"
                    >
                      {showPassword ? <VisibilityOff /> : <Visibility />}
                    </IconButton>
                  </InputAdornment>
                }
                label="Password"
              />
            </FormControl>
          </div>
          <br />
          <div className="submit">
            <Button className="submit__button" type="submit" variant="outlined">
              Register
            </Button>
          </div>
          <br />
        </form>
      }
    </div>
  );
};

export default Register;
