import { 
  ValidateEmail,
  ValidatePhone, 
  ValidatePassword,
  ValidateRequired
} from './CredentialsValidate'

export function SettingsValidate({ settings }) {
  const { email, phone, password, firstName } = settings
  return ValidateEmail(email) && ValidatePhone(phone) && ValidatePassword(password) && ValidateRequired(firstName)
}
