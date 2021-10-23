import { Role } from '../Types/Role'

const Roles = Object.values(Role)

export function ValidatePhone(phone) {
  return /^\+(?:[0-9] ?){6,14}[0-9]$/.test(phone)
}

export function ValidateEmail(email) {
  return /[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/.test(email)
}

export function ValidatePassword(password) {
  return /(?=.*[A-Z])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8,}/.test(password)
}

export function ValidateRole(role) {
  return Roles.includes(role)
}

export function ValidateRequired(str) {
  return Boolean(str)
}

export function SignInValidate({ login, password, role }) {
  return (
    (ValidateEmail(login) || ValidatePhone(login)) 
    && ValidatePassword(password) 
    && ValidateRole(role)
  )
}

export function SignUpValidate({ firstName, email, phone, password, role }) {
  return (
    ValidateEmail(email) &&
    ValidatePhone(phone) &&
    ValidatePassword(password) &&
    ValidateRole(role) &&
    ValidateRequired(firstName)
  )
}