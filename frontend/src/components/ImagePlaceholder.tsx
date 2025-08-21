import { Building } from 'lucide-react';

interface ImagePlaceholderProps {
  className?: string;
  text?: string;
}

export function ImagePlaceholder({ 
  className = "w-full h-48", 
  text = "Imagen no disponible" 
}: ImagePlaceholderProps) {
  return (
    <div className={`${className} bg-gray-200 flex items-center justify-center`}>
      <div className="text-center text-gray-500">
        <Building className="h-12 w-12 mx-auto mb-2" />
        <p className="text-sm">{text}</p>
      </div>
    </div>
  );
}