import { useEffect } from 'react';
import { useRouter } from 'next/router';
import { auth } from '@/lib/firebase';
import LoginForm from '@/components/auth/LoginForm';

export default function LoginPage() {
  const router = useRouter();

  useEffect(() => {
    const unsubscribe = auth.onAuthStateChanged((user) => {
      if (user) {
        router.push('/');
      }
    });

    return () => unsubscribe();
  }, [router]);

  return <LoginForm />;
} 