function QuakeMap::onRemove(%this)
{
	%this.data.delete();

    %this.authors.delete();
    %this.cameras.delete();
}

function QuakeMap::getCredits(%this)
{
    %length = %this.authors.getLength();

	if (%length < 2) {
		return %this.authors.item[0];
	}

	for (%i = 0; %i < %length - 1; %i++) {
		%partial = %partial @ (%i ? ", " : "") @ %this.authors.item[%i];
	}

	return %partial SPC "and" SPC %this.authors.item[%length - 1];
}