import { useState } from 'react'
import Password from '../Components/Password'

import { ValidateRole, SignUpValidate as Validate } from '../Validators/CredentialsValidate'
import { SignUpRequest as Request } from '../Services/Request'

import { Role } from '../Types/Role'

import './SignUp.css'

const Roles = Object.values(Role)

const emptyState = {
    email: null,
    password: null,
    phone: null,
    firstName: null,
    lastName: null,
    role: Role.Customer
}

function SignUp({ onSignUp, host }) {
  if (!host) {
    throw new Error("Host prop is not provided")
  }

  const [state, setState] = useState(emptyState)

  const [error, setError] = useState()

  const errorMessage = <p style={{color: '#FF0000'}}>{error && (error.toString ? error.toString() : "Error occurred") }</p>

  const buttonClicked = () => {
    if (!Validate(state)) {
      setError("Provided data is not valid")
      return
    }
    Request(state, json => {
      onSignUp(json)  
    }, error => {
      setError("Request failed: " + error)
    }, host)
  }

  const firstNameChanged = e => {
    setError()
    setState(prevState => {
      return { ...prevState, firstName: e.target.value }
    })
  }

  const lastNameChanged = e => {
    setError()
    setState(prevState => {
      return { ...prevState, lastName: e.target.value }
    })
  }

  const emailChanged = e => {
    setError()
    setState(prevState => {
      return { ...prevState, email: e.target.value }
    })
  }

  const phoneChanged = e => {
    setError()
    setState(prevState => {
      return { ...prevState, phone: e.target.value }
    })
  }

  const passwordChanged = e => {
    setError()
    setState(prevState => {
      return { ...prevState, password: e.target.value }
    })
  }

  const selectUpdated = e => {
    if (!ValidateRole(e.target.value)) {
      throw new Error("Invalid role!")
    }

    setError()
    setState(prevState => {
      return { ...prevState, role: e.target.value}
    })
  }

  return (
    <div className="signup-form-container">
      <h1>Sign up</h1>

      <div className="field-container">
        <label>First Name</label><input type="text" placeholder="First Name" onChange={firstNameChanged} />
      </div>
      <div className="field-container">
        <label>Last Name</label><input type="text" placeholder="Last Name" onChange={lastNameChanged} />
      </div>
      <div className="field-container">
        <label>Email</label><input type="email" placeholder="imienazwisko@poczta.pl" onChange={emailChanged} />
      </div>
      <div className="field-container">
        <label>Phone</label><input type="phone" placeholder="+48888888888" onChange={phoneChanged} />
      </div>
      <Password onChange={passwordChanged} />
      <div className="field-container">
        <label>Role</label>
        <select onChange={selectUpdated}>
          { Roles.map((role, id) => <option key={id} value={role}>{role}</option>) }
        </select>
      </div>
      <button onClick={buttonClicked}>Sign up</button>
      { error && errorMessage }
    </div>
  )
}

export default SignUp
